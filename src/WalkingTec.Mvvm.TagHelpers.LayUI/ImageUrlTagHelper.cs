using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WalkingTec.Mvvm.Core;

namespace WalkingTec.Mvvm.TagHelpers.LayUI
{
    [HtmlTargetElement("wt:imageUrl", Attributes = REQUIRED_ATTR_NAME, TagStructure = TagStructure.WithoutEndTag)]
    public class ImageUrlTagHelper : BaseElementTag
    {
        protected const string REQUIRED_ATTR_NAME = "field";
        public ModelExpression Field { get; set; }
        public string HttpUrl { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var vm = context.Items["model"] as BaseVM;
            output.TagName = "img";
            output.TagMode = TagMode.SelfClosing;
            output.Attributes.Add("name", Field.Name + "img");
            output.Attributes.Add("id", Id + "img");
            if (!string.IsNullOrEmpty(HttpUrl))
                output.Attributes.Add("src", HttpUrl);
            base.Process(context, output);

        }
    }
}
