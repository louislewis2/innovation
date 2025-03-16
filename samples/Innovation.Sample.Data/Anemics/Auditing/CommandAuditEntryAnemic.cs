namespace Innovation.Sample.Data.Anemics.Auditing
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CommandAuditEntries")]
    public class CommandAuditEntryAnemic : AnemicBase
    {
        #region Constructor

        public CommandAuditEntryAnemic(
            Guid id,
            string name,
            string commandContext,
            string commandResultContext,
            string correlationId,
            long runtimeMilliSeconds) : base(id)
        {
            this.Name = name;
            this.CommandContext = commandContext;
            this.CommandResultContext = commandResultContext;
            this.CorrelationId = correlationId;
            this.RuntimeMilliSeconds = runtimeMilliSeconds;
        }

        #endregion Constructor

        #region Properties

        public string Name { get; set; }
        public string CommandContext { get; set; }
        public string CommandResultContext { get; set; }
        public string CorrelationId { get; set; }
        public long RuntimeMilliSeconds { get; set; }

        #endregion Properties

        #region Methods

        public static CommandAuditEntryAnemic New(
            Guid id,
            string name,
            string commandContext,
            string commandResultContext,
            string correlationId,
            long runtimeMilliSeconds)
        {
            return new CommandAuditEntryAnemic(
                id: id,
                name: name,
                commandContext: commandContext,
                commandResultContext: commandResultContext,
                correlationId: correlationId,
                runtimeMilliSeconds: runtimeMilliSeconds);
        }

        #endregion Methods
    }
}
