using System.ComponentModel.DataAnnotations;
using App.Domain.Entities;
using FluentAssertions;

namespace App.Tests.Unit;

public sealed class TargetUnitValidationTests {
    [Fact]
    [Trait("Category", "Unit")]
    public void 識別子が最大長を超える場合は検証エラーになる() {
        var target = new TargetUnit {
            pandaid = new string('A', 11)
        };
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            target,
            new ValidationContext(target),
            validationResults,
            validateAllProperties: true);

        isValid.Should().BeFalse();
        validationResults.Should().ContainSingle(result =>
            result.MemberNames.Contains(nameof(TargetUnit.pandaid)));
    }
}
