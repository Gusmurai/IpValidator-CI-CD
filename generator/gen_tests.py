import yaml
import argparse
import os
from pathlib import Path
from typing import Dict, List, Any

# 1. ШАБЛОН ФАЙЛА (Добавили #nullable disable в самое начало)
TEST_FILE_TEMPLATE = """#nullable disable
// =============================================================
// AUTO-GENERATED TESTS. DO NOT EDIT MANUALLY.
// Source: {spec_source}
// Generator: gen_tests.py v1.0
// =============================================================
using System;
using System.Collections.Generic;
using NUnit.Framework;
using IpValidator.Core;

namespace Tests
{{
    [TestFixture]
    [Description("Автоматически сгенерированные тесты для {module_name}")]
    public class {module_name}GeneratedTests
    {{
        private I{module_name} _sut;

        [SetUp]
        public void SetUp()
        {{
            // Инициализация тестируемой системы (SUT)
            _sut = new {module_name}();
        }}

{test_methods}
    }}
}}
"""

# 2. ШАБЛОН МЕТОДА ТЕСТА
TEST_METHOD_TEMPLATE = """
        [Test]
        [Description("Класс эквивалентности: {case_desc}")]
        {test_cases}
        public void Test_{method_name}_{case_name}(string input)
        {{
            // === Arrange ===
            // Предусловие: {pre}
            // Ожидаемый результат: {expected}

            // === Act & Assert ===
{logic_block}
        }}
"""

TEST_CASE_TEMPLATE = "[TestCase({inputs})]"

def load_spec(spec_path: str) -> Dict[str, Any]:
    with open(spec_path, "r", encoding="utf-8") as f:
        return yaml.safe_load(f)

def format_csharp_input(value: Any) -> str:
    if value is None: return "null"
    if isinstance(value, str): return f"{value}"
    if isinstance(value, bool): return "true" if value else "false"
    return str(value)

def generate_method_tests(method_data: Dict[str, Any]) -> List[str]:
    case_blocks = []
    method_name = method_data["name"]
    
    for eq_class in method_data.get("equivalence_classes", []):
        inputs_str = ", ".join(format_csharp_input(inp) for inp in eq_class["inputs"])
        expected_val = str(eq_class.get("expected", "")).lower()
        
        # Логика Тестового Оракула
        if "exception" in expected_val:
            # Если в YAML ожидается Exception, генерируем Throws
            logic_block = f"            Assert.Throws<ArgumentException>(() => _sut.{method_name}(input));"
        else:
            # Для обычных методов формируем Act и затем Assert
            if method_data["signature"].startswith("void"):
                logic_block = f"            _sut.{method_name}(input);\n"
                logic_block += f'            Assert.Pass("Метод выполнен без ошибок");'
            else:
                logic_block = f"            var result = _sut.{method_name}(input);\n"
                if expected_val == "true":
                    logic_block += "            Assert.That(result, Is.True);"
                elif expected_val == "false":
                    logic_block += "            Assert.That(result, Is.False);"
                else:
                    logic_block += f'            Assert.Pass("Результат получен: " + result);'

        case_blocks.append(
            TEST_METHOD_TEMPLATE.format(
                case_desc=eq_class["case"],
                test_cases=TEST_CASE_TEMPLATE.format(inputs=inputs_str),
                method_name=method_name,
                case_name=eq_class["case"].replace(" ", "_").replace("(", "").replace(")", "").replace("-", ""),
                pre=method_data["pre"],
                expected=eq_class["expected"],
                logic_block=logic_block,
                post=method_data["post"]
            )
        )
    return case_blocks

def render_and_save(spec: Dict[str, Any], config: Dict[str, Any]) -> None:
    module_name = spec["module"]
    test_methods = []
    for method in spec["methods"]:
        test_methods.extend(generate_method_tests(method))
        
    tests_block = "\n".join(test_methods)
    
    file_content = TEST_FILE_TEMPLATE.format(
        spec_source=config.get("spec_path", "N/A"),
        module_name=module_name,
        test_methods=tests_block
    )
    
    out_dir = Path(config.get("output_dir", "tests/IpValidator.Tests"))
    out_dir.mkdir(parents=True, exist_ok=True)
    output_file = out_dir / f"{module_name}GeneratedTests.cs"
    
    output_file.write_text(file_content, encoding="utf-8")
    print(f"[OK] Сгенерирован файл: {output_file}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--config", default="config.yaml")
    args = parser.parse_args()
    
    with open(args.config, "r", encoding="utf-8") as f:
        config = yaml.safe_load(f)
        
    spec_data = load_spec(config["spec_path"])
    render_and_save(spec_data, config)
    print("[OK] Готово.")