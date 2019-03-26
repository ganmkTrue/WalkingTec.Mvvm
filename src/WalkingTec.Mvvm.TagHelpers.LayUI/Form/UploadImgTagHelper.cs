using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WalkingTec.Mvvm.Core;


namespace WalkingTec.Mvvm.TagHelpers.LayUI
{
    [HtmlTargetElement("wt:uploadImg", Attributes = REQUIRED_ATTR_NAME, TagStructure = TagStructure.WithoutEndTag)]
    public class UploadImgTagHelper : BaseFieldTag
    {
        public int FileSize { get; set; }
        public UploadTypeEnum UploadType { get; set; }
        public string CustomType { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            output.Attributes.Add("id", Id + "button");
            output.Attributes.Add("name", Id + "button");
            output.Attributes.Add("class", "layui-btn layui-btn-sm");
            output.Attributes.Add("type", "button");
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent("选择文件");
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

            var url = "/_Framework/Upload";
            var viewUrl = "/_Framework/ViewFile/";
            var _viewUrlpart = "";
            if (vm != null)
            {
                url += $"?_DONOT_USE_CS={vm.CurrentCS}";
                _viewUrlpart = $"?_DONOT_USE_CS={vm.CurrentCS}";
            }

            output.PreElement.SetHtmlContent($@"
<label id='{Id}label'></label>
");

            output.PostElement.SetHtmlContent($@"
<input type='hidden' id='{Id}' name='{Field.Name}' value='{Field.Model}' {(Field.Metadata.IsRequired ? " lay-verify=required" : string.Empty)} />
<script>
    function {Id}DoDelete(fileid){{
        $('#{Id}').parents('form').append(""<input type='hidden' id='DeletedFileIds' name='DeletedFileIds' value='""+fileid+""' />"");
        $('#{Id}label').html('');
        $('#{Id}').val('');
    }}
    var index = 0;
 
    //普通图片上传
    var uploadInst = layui.upload.render({{
        elem: '#{Id}button'
        ,url: '{url}'
        ,size: {FileSize}
        ,accept: 'file'
        ,exts: '{ext}'
        ,before: function(obj){{
            index = layui.layer.load(2);
        }}
        ,done: function(res){{
            layui.layer.close(index);
            if(res.Data.Id == ''){{
                layui.layer.msg('上传失败');
            }}
            else{{
                $('#{Id}').val(res.Data.Id);
                var del = ""<button class='layui-btn layui-btn-sm layui-btn-danger' type='button' id='{Id}del' style='color:white'>""+res.Data.Name+""  删除</button>"";
                $('#{Id}label').html(del);
                $('#{Id}del').on('click',function(){{
                    {Id}DoDelete(res.Data.Id);
                    $('#{Id}label').parent().find('#FileObject').remove();
                }});
               
                $.ajax({{
                    cache: false,
                    type: 'GET',
                    url: '{viewUrl}'+$('#{Id}').val()+'{_viewUrlpart}',
                    async: false,
                    success: function(data) {{
                      $('#{Id}label').parent().find('#FileObject').remove();
                      $('#{Id}label').parent().append(data)
                     }}
                }});
           
                
            }}
        }}
        ,error: function(){{
            layui.layer.close(index);
        }}
    }});
</script>
");
            if (Field.Model != null && Field.Model.ToString() != Guid.Empty.ToString())
            {

                var geturl = $"/_Framework/GetFileName/{Field.Model}";
                if (vm != null)
                {
                    geturl += $"?_DONOT_USE_CS={vm.CurrentCS}";
                }

                output.PostElement.AppendHtml($@"
<script>
    $.ajax({{
        cache: false,
        type: 'GET',
        url: '{geturl}',
        async: false,
        success: function(data) {{
            var del = ""<button class='layui-btn layui-btn-sm layui-btn-danger' type='button' id='{Id}del' style='color:white'>""+data+""  删除</button>"";
            $('#{Id}label').html(del);
            $('#{Id}del').on('click',function(){{
                {Id}DoDelete('{Field.Model}');
            }});
        }}
    }});
</script>
");

                if (Field.Model != null && Field.Model.ToString() != Guid.Empty.ToString())
                {
                    var _imghtml= $"/_Framework/ViewFile/{Field.Model}";
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
            $('#{Id}label').parent().append(data)
            }}
    }});
</script>
");

                }

                }
            base.Process(context, output);
        }
    }
}
