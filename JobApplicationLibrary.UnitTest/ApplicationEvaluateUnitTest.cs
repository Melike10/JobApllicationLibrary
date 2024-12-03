using JobApllicationLibrary;
using JobApllicationLibrary.Models;
using JobApllicationLibrary.Services;
using Moq;
using FluentAssertions;

namespace JobApplicationLibrary.UnitTest
{
    public class ApplicationEvaluateUnitTest
    {
        // testleri isimlendirirken kullanýlacak yöntem
        //unitOfWork_ExpectedResult_Condition
        [Test]
        public void Applicant_ShouldTransferredToAutoReject_WithUnderAge()
        {
            //Arrange
            var evaulator = new ApplicationEvaluator(null);
            var form = new JobApplication()
            {
                Applicant = new Applicant
                {
                    Age = 17
                }
            };
            //Action
            var appResult = evaulator.Evaluate(form);

            //Result
            // Assert.AreEqual(ApplicationResult.AutoReject, appResult);
            //fluentassertions sayesinde daha okunabilir olmasýný saðladýk.
            appResult.Should().Be(ApplicationResult.AutoReject);
        }

        [Test]
        public void Applicant_ShouldTransferredToAutoReject_WithNoStack()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            // burada sahte verimiz ne deðer alýrsa alsýn return true gelsin isvalid methodundan dedik
            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(i => i.CountryProvider.CountryData.Country).Returns("TURKEY");
            var evaulator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 23, IdentyNumber = "" },
                TechStackList = new List<string>() { "" },
                OfficeLocation = "ISTANBUL"



            };



            //Action
            var appResult = evaulator.Evaluate(form);

            //Result
            //Assert.AreEqual(ApplicationResult.AutoReject, appResult);
            appResult.Should().Be(ApplicationResult.AutoReject);
        }

        [Test]
        public void Applicant_ShouldTransferredToAutoAccepted_WithStackListAndExperience()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            // burada sahte verimiz ne deðer alýrsa alsýn return true gelsin isvalid methodundan dedik
            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(i => i.CountryProvider.CountryData.Country).Returns("TURKEY");
            var evaulator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 38, IdentyNumber = "123" },
                TechStackList = new List<string>() { "C#", "Asp.net core", "RabbitMq", "Ms Sql", "Visual Studio" },
                YearsOfExperience = 16,
                OfficeLocation = "ISTANBUL"


            };



            //Action
            var appResult = evaulator.Evaluate(form);

            //Result
            //Assert.AreEqual(ApplicationResult.AutoAccept, appResult);
            appResult.Should().Be(ApplicationResult.AutoAccept);
        }
        [Test]
        public void Applicant_ShouldTransferredHR_WithInvalidIdentityumber()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();

            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(false);
            mockValidator.Setup(i => i.CountryProvider.CountryData.Country).Returns("TURKEY");
            var evaulator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 38 },
                OfficeLocation = "ISTANBUL"



            };



            //Action
            var appResult = evaulator.Evaluate(form);

            //Result
            // Assert.AreEqual(ApplicationResult.TransferredToHR, appResult);
            appResult.Should().Be(ApplicationResult.TransferredToHR);
        }
        [Test]
        public void Applicant_ShouldTransferredCTO_WithOfficeLocation()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();

            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(false);
            mockValidator.Setup(i => i.CountryProvider.CountryData.Country).Returns("TURKEY");
            var evaulator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 38 },
                OfficeLocation = "ANKARA"

            };


            //Action
            var appResult = evaulator.Evaluate(form);

            //Result
            //Assert.AreEqual(ApplicationResult.TransferredToCTO, appResult);
            appResult.Should().Be(ApplicationResult.TransferredToCTO);
        }
        [Test]
        public void Applicant_ShouldTransferredCTO_WithCountry()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();

            mockValidator.Setup(i => i.CountryProvider.CountryData.Country).Returns("SPAIN");
            var evaulator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 38 },
                OfficeLocation = "ANKARA"

            };

            //Action
            var appResult = evaulator.Evaluate(form);

            //Result
            // Assert.AreEqual(ApplicationResult.TransferredToCTO, appResult);
            appResult.Should().Be(ApplicationResult.TransferredToCTO);
        }
        [Test]
        public void Application_ValidationModeDetailed_WithOver50()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();

            mockValidator.Setup(i => i.CountryProvider.CountryData.Country).Returns("SPAIN");
            var evaulator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 68 },
            };

            //Action
            var appResult = evaulator.Evaluate(form);

            //Result
            Assert.AreEqual(ValidationMode.Detail, form.ValidationMode);
        }

        [Test]
        public void Application_ThrowsArgumentNullException_WithNullApplicant()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            var evaulator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication();
            //Action
            Action appResultAction = () =>evaulator.Evaluate(form);

            //Result    
            appResultAction.Should().Throw<ArgumentNullException>();
        }
        //mock verify ile Ýsvalid metodu çalýþmýþ mý onu görmüþ olduðumuz bir senaryo
        [Test]
        public void Application_IsValidCalled_WithDefaultValue()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(i => i.CountryProvider.CountryData.Country).Returns("TURKEY");
            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant=new Applicant
                {
                    Age=19,
                    IdentyNumber="123"
                },
                OfficeLocation = "ISTANBUL"
            };
            var appResult = evaluator.Evaluate(form);
            mockValidator.Verify(i => i.IsValid(It.IsAny<string>()), "IsValid method should be called 123");

        }

        [Test]
        // isvalid hiç çaðrýlmazsa test edildi.
        public void Application_IsValidNeverCalled_WithYoungAge()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(i => i.CountryProvider.CountryData.Country).Returns("TURKEY");
            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant
                {
                    Age = 15
                }
                ,
                OfficeLocation = "ISTANBUL"
            };
            var appResult = evaluator.Evaluate(form);
            // Times.Exactly(0) diyerek ya da baþka sayýlarak vererek kaç kez de çaðrýlmasý gerekitðini bakabilirz.
            mockValidator.Verify(i => i.IsValid(It.IsAny<string>()), Times.Never);

        }
    }
}