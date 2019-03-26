using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WalkingTec.Mvvm.Core;

namespace WalkingTec.Mvvm.TagHelpers.LayUI
{
    [HtmlTargetElement("wt:ImgView", Attributes = REQUIRED_ATTR_NAME, TagStructure = TagStructure.WithoutEndTag)]
    public class ImgViewTagHelper : BaseFieldTag
    {
        public int FileSize { get; set; }
        public UploadTypeEnum UploadType { get; set; }
        public string CustomType { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";
            output.Attributes.Add("id", Id + "input");
            output.Attributes.Add("type", "hidden");
            string ext = "";
            if (string.IsNullOrEmpty(CustomType))
            {
                switch (UploadType)
                {
                    case UploadTypeEnum.ImageFile:
                        ext = "jpg|jpeg|gif|bmp|png|tif";
                        break;
                    default:
                        break;
                }

            }
            else
            {
                ext = CustomType;
            }

            var vm = context.Items["model"] as BaseVM;

            if (Field.Model != null && Field.Model.ToString() != Guid.Empty.ToString())
            {
                var _imghtml = $"/_Framework/ViewFile/{Field.Model}";
                if (vm != null)
                {
                    _imghtml += $"?_DONOT_USE_CS={vm.CurrentCS}";
                }

                output.PostElement.AppendHtml($@"
                <script>
                    $.ajax({{
                        cache: false,
                        type: 'GET',
                        url: '{_imghtml}',
                        async: false,
                        success: function(data) {{
                            $('#{Id}input').parent().append(data)
                        }}
                    }});
                </script>
                ");

            }
            base.Process(context, output);
        }

    }
}
