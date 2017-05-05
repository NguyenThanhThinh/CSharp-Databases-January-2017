namespace FootballData.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FootballModels;

    class BetConfiguration : EntityTypeConfiguration<Bet>
    {
        public BetConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.DateTime).IsRequired();
            Property(x => x.BetMoney).IsRequired();
            Property(x => x.UserId).IsRequired();
        }
    }
}
