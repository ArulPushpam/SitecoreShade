using Sitecore;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;


namespace Feature.CommandTemplate.Pipeline
{
    public class GetDialog
    {
        public void Process()
        {
            UrlString urlString = new UrlString(UIUtil.GetUri("control:ImportSiteWizard"));
            SheerResponse.ShowModalDialog(urlString.ToString(), "400", "480");
        }
    }
}