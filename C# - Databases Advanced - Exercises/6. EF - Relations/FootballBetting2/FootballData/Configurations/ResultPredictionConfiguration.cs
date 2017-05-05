namespace FootballData.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FootballModels;

    class ResultPredictionConfiguration : EntityTypeConfiguration<ResultPrediction>
    {
        public ResultPredictionConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Prediction).IsRequired();
        }
    }
}
