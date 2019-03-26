﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.ApiDemo.Models;


namespace WalkingTec.Mvvm.ApiDemo.ViewModels.SchoolVMs
{
    public class SchoolVM : BaseCRUDVM<School>
    {

        public SchoolVM()
        {
            SetInclude(x => x.Place);
            SetInclude(x => x.Place2);
            SetInclude(x => x.Place2.Parent.Parent);
        }

        protected override void InitVM()
        {
        }

        public override void DoAdd()
        {           
            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }
    }
}
