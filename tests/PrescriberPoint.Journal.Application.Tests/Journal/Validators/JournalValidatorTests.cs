
using FluentValidation.TestHelper;

namespace PrescriberPoint.Journal.Application.Tests.Journal.Validators;

public class JournalValidatorTests
{
    private readonly JournalValidator _journalValidator;

    public JournalValidatorTests() {
        _journalValidator = new JournalValidator();
    }

    [Fact]
    public void TestValidate_ShouldHaveValidationErrorFor_UserId_When_Its_0()
    {
        var validationResult = _journalValidator.TestValidate(
            new Domain.Journal{
                UserId = 0
            });

        validationResult.ShouldHaveValidationErrorFor(x=>x.UserId);
    }

    
    [Fact]
    public void TestValidate_ShouldHaveValidationErrorFor_UserId_WhenIts_Negative()
    {
        var validationResult = _journalValidator.TestValidate(
            new Domain.Journal{
                UserId = -1
            });

        validationResult.ShouldHaveValidationErrorFor(x=>x.UserId);
    }

    [Fact]
    public void TestValidate_ShouldNotHaveValidationErrorFor_UserId_WhenItsGreaterThan_0()
    {
        var validationResult = _journalValidator.TestValidate(
            new Domain.Journal{
                UserId = 1
            });

        validationResult.ShouldNotHaveValidationErrorFor(x=>x.UserId);
    }

    [Fact]
    public void TestValidate_ShouldHaveValidationErrorFor_Patient_WhenIts_Empty()
    {
        var validationResult = _journalValidator.TestValidate(
            new Domain.Journal{
                Patient = string.Empty
            });

        validationResult.ShouldHaveValidationErrorFor(x=>x.Patient);
    }

    [Fact]
    public void TestValidate_ShouldHaveValidationErrorFor_Patient_WhenItsLengthIsGreaterThan_50()
    {
        var validationResult = _journalValidator.TestValidate(
            new Domain.Journal{
                Patient = new string('0', 51)
            });

        validationResult.ShouldHaveValidationErrorFor(x=>x.Patient);
    }

    [Fact]
    public void TestValidate_ShouldNotHaveValidationErrorFor_Patient_WhenItsLengthIsLessOrEqualThan_50()
    {
        var validationResult = _journalValidator.TestValidate(
            new Domain.Journal{
                Patient = new string('0', 50)
            });

        validationResult.ShouldNotHaveValidationErrorFor(x=>x.Patient);
    }

    [Fact]
    public void TestValidate_ShouldNotHaveValidationErrorFor_Note_WhenIts_Empty()
    {
        var validationResult = _journalValidator.TestValidate(
            new Domain.Journal{
                Note = string.Empty
            });

        validationResult.ShouldNotHaveValidationErrorFor(x=>x.Note);
    }

    [Fact]
    public void TestValidate_ShouldHaveValidationErrorFor_Note_WhenItsLengthIsGreaterThan_200()
    {
        var validationResult = _journalValidator.TestValidate(
            new Domain.Journal{
                Note = new string('0', 201)
            });

        validationResult.ShouldHaveValidationErrorFor(x=>x.Note);
    }

    [Fact]
    public void TestValidate_ShouldNotHaveValidationErrorFor_Note_WhenItsLengthIsLessOrEqualThan_200()
    {
        var validationResult = _journalValidator.TestValidate(
            new Domain.Journal{
                Note = new string('0', 200)
            });

        validationResult.ShouldNotHaveValidationErrorFor(x=>x.Note);
    }
}