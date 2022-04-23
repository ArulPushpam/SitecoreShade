using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;

namespace Feature.CommandTemplate.Commands
{
    public abstract class CommandBase : Command
    {
        public override CommandState QueryState(CommandContext context)
        {
            Assert.ArgumentNotNull((object)context, nameof(context));
            Item[] items = context.Items;
            if (items.Length != 1)
                return CommandState.Hidden;
            Item obj = items[0];
            return CommandState.Enabled;
        }
    }
}