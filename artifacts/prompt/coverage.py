import json

with open("test-results.json") as f:
    data = json.load(f)

runs = [r for r in data if "run" in r]
total_asserts = sum(r["test"]["assertions_total"] for r in runs if "test" in r)
failed_asserts = sum(r["test"]["assertions_failed"] for r in runs if "test" in r)
passed_runs = sum(1 for r in runs if r["test"]["status"] == "pass")
skipped_runs = sum(1 for r in runs if r["test"]["status"] == "skip")

coverage = ((passed_runs - skipped_runs) / len(runs)) * 100
print(f"Estimated Test Coverage: {coverage:.2f}%")
print(f"Assertions Passed: {total_asserts - failed_asserts}/{total_asserts}")
