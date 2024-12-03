using JobApllicationLibrary.Models;
using JobApllicationLibrary.Services;

namespace JobApllicationLibrary
{
    public class ApplicationEvaluator
    {
        private const int minAge = 18;
        private const int autoAcceptedYearsOfExperience = 15;
        private List<String> techStackList = new List<String>() { "C#","RabbitMq","Microservice","Visual Studio","Ms Sql","Asp.net Core"};
        // dependecy injection
        private IIdentityValidator _identyValidator;

        public ApplicationEvaluator(IIdentityValidator identyValidator)
        {
            _identyValidator = identyValidator; 
        }
        public ApplicationResult Evaluate(JobApplication form)
        {
            if (form.Applicant is null)
                throw new ArgumentNullException();

            if (form.Applicant.Age < minAge)
                return ApplicationResult.AutoReject;

            form.ValidationMode = form.Applicant.Age > 50 ? ValidationMode.Detail : ValidationMode.Quick;

            // hiyerarşik yapıda datamız
            if (_identyValidator.CountryProvider.CountryData.Country != "TURKEY")
                return ApplicationResult.TransferredToCTO;

            if (form.OfficeLocation != "ISTANBUL")
                return ApplicationResult.TransferredToCTO;
            

            var validIdentity= _identyValidator.IsValid(form.Applicant.IdentyNumber);
            // eğer geçerli bir kimlilk numarası yoksa HR onları arasın
            if (!validIdentity)
                return ApplicationResult.TransferredToHR;

            var stackRate = GetStackSimilarityRate(form.TechStackList);

            if(stackRate < 25)
                return ApplicationResult.AutoReject;
            if(stackRate>75 && form.YearsOfExperience<autoAcceptedYearsOfExperience)
                return ApplicationResult.AutoAccept;

        
            return ApplicationResult.AutoAccept;
        }
        private int GetStackSimilarityRate(List<string> techStack)
        {
            var matchedCount = techStack.Where(i=>techStackList.Contains(i,StringComparer.OrdinalIgnoreCase)).Count();
            return (int) ((double)matchedCount/techStackList.Count()*100);
        }
    }

   

    public enum ApplicationResult
    {
        AutoReject,
        TransferredToHR,
        TransferredToLead,
        TransferredToCTO,
        AutoAccept


    }
}
