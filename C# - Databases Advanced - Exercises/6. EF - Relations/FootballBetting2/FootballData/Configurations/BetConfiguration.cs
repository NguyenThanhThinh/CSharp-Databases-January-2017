namespace FootballData.Configurations
{
    using FootballModels;
    using System.Data.Entity.ModelConfiguration;

    internal class BetConfiguration : EntityTypeConfiguration<Bet>
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