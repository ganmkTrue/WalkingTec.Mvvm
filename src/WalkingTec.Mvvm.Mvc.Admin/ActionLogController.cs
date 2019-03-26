﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Mvc;
using WalkingTec.Mvvm.Mvc.Admin.ViewModels.ActionLogVMs;

namespace WalkingTec.Mvvm.Admin.Api
{

    [ActionDescription("日志管理")]
    [ApiController]
    [Route("api/_ActionLog")]
	public class ActionLogController : BaseApiController
    {
        [ActionDescription("搜索")]
        [HttpPost("Search")]
		public string Search(ActionLogSearcher searcher)
        {
            var vm = CreateVM<ActionLogListVM>();
            vm.Searcher = searcher;
            return vm.GetJson();
        }

        [ActionDescription("获取")]
        [HttpGet("{id}")]
        public ActionLog Get(Guid id)
        {
            var vm = CreateVM<ActionLogVM>(id);
            return vm.Entity;
        }

        [ActionDescription("新建")]
        [HttpPost("Add")]
        public IActionResult Add(ActionLogVM vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorJson());
            }
            else
            {
                vm.DoAdd();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.GetErrorJson());
                }
                else
                {
                    return Ok(vm.Entity);
                }
            }

        }

        [ActionDescription("修改")]
        [HttpPut("Edit")]
        public IActionResult Edit(ActionLogVM vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorJson());
            }
            else
            {
                vm.DoEdit(true);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.GetErrorJson());
                }
                else
                {
                    return Ok(vm.Entity);
                }
            }
        }

        [ActionDescription("删除")]
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var vm = CreateVM<ActionLogVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorJson());
            }
            else
            {
                return Ok(vm.Entity);
            }

        }

        [HttpPost("BatchDelete")]
        [ActionDescription("批量删除")]
        public IActionResult BatchDelete(Guid[] ids)
        {
            var vm = CreateVM<ActionLogBatchVM>();
            if (ids != null && ids.Count() > 0)
            {
                vm.Ids = ids;
            }
            else
            {
                return Ok();
            }
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return BadRequest(ModelState.GetErrorJson());
            }
            else
            {
                return Ok(ids.Count());
            }
        }


        [ActionDescription("导出")]
        [HttpPost("ExportExcel")]
        public IActionResult ExportExcel(ActionLogSearcher searcher)
        {
            var vm = CreateVM<ActionLogListVM>();
            vm.Searcher = searcher;
            vm.SearcherMode = ListVMSearchModeEnum.Export;
            var data = vm.GenerateExcel();
            return File(data, "application/vnd.ms-excel", $"Export_ActionLog_{DateTime.Now.ToString("yyyy-MM-dd")}.xls");
        }

        [ActionDescription("勾选导出")]
        [HttpPost("ExportExcelByIds")]
        public IActionResult ExportExcelByIds(Guid[] ids)
        {
            var vm = CreateVM<ActionLogListVM>();
            if (ids != null && ids.Count() > 0)
            {
                vm.Ids = new List<Guid>(ids);
                vm.SearcherMode = ListVMSearchModeEnum.CheckExport;
            }
            var data = vm.GenerateExcel();
            return File(data, "application/vnd.ms-excel", $"Export_ActionLog_{DateTime.Now.ToString("yyyy-MM-dd")}.xls");
        }
    }
}
