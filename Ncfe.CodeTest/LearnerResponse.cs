namespace Ncfe.CodeTest
{
    public class LearnerResponse
    {
        public LearnerResponse()
        {
            Learner = new Learner();
        }
        public bool IsArchived { get; set; }

        public Learner Learner { get; set; }
    }
}
