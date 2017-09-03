namespace FootballData.Configurations
{
    using FootballModels;
    using System.Data.Entity.ModelConfiguration;

    internal class ResultPredictionConfiguration : EntityTypeConfiguration<ResultPrediction>
    {
        public ResultPredictionConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Prediction).IsRequired();
        }
    }
}