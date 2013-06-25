﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AHome.Models
{
    public class RogerInitializer : DropCreateDatabaseIfModelChanges<RogerContext>
    {
        protected override void Seed(RogerContext context)
        {
            var newsTypes = new List<NewsType> 
            {
                new NewsType(){ Name="First"},
                new NewsType(){ Name = "Second"},
                new NewsType(){ Name = "Third"},
                new NewsType(){ Name = "Forth"},
                new NewsType(){ Name = "Fifth"},
                new NewsType(){ Name = "Sixth"}
            };
            newsTypes.ForEach(s => context.NewsTypes.Add(s));

            var craftTypes = new List<CraftType> 
            {
                new CraftType(){ Name="潮州木雕", level = 1},
                new CraftType(){ Name = "花木雕",level =2},
                new CraftType(){ Name = "红花木雕",level = 3},
                new CraftType(){ Name = "潮州刺绣",level=1},
                new CraftType(){ Name = "石雕",level = 1},
                new CraftType(){Name = "大吴泥雕",level = 1},
                new CraftType(){Name = "木雕",level=2},
            };
            craftTypes.ForEach(s => context.CraftTypes.Add(s));

            context.SaveChanges();
        }
    }
}