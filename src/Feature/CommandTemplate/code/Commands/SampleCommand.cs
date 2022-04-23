using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System;

namespace Feature.CommandTemplate.Commands
{
    public class SampleCommand : CommandBase
    {
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, nameof(context));
            if (context.Items.Length < 1)
            {
                return;
            }
            Item obj = context.Items[0];

            Sitecore.Context.ClientPage.Start("sample.ui");
        }
    }
}