using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Repository;
using VueExample.ViewModels;

namespace VueExample.Providers
{
    public class DefectProvider : Repository<Defect>, IDefectProvider
    {
        public int InsertNewDefect(Defect defect)
        {
            using (VisualControlContext visualControlContext = new VisualControlContext())
            {
                visualControlContext.Defects.Add(defect);
                visualControlContext.SaveChanges();
            }
            return defect.DefectId;
        }

       

        public void DeleteById(int defectId)
        {
            using (VisualControlContext visualControlContext = new VisualControlContext())
            {
                var deletedDefect = visualControlContext.Defects.FirstOrDefault(x => x.DefectId == defectId);
                if (deletedDefect != null) visualControlContext.Defects.Remove(deletedDefect);
                visualControlContext.SaveChanges();
            }
        }

        public int GetDuplicate(long dieId, int stageId, int defectTypeId)
        {
            Defect duplicate;
            using (VisualControlContext visualControlContext = new VisualControlContext())
            {
                duplicate = visualControlContext.Defects.FirstOrDefault(x =>
                    x.DieId == dieId && x.DefectTypeId == defectTypeId && x.StageId == stageId);
            }

            return duplicate?.DefectId ?? 0;
        }

        public List<Defect> GetByWaferId(string waferId)
        {
            using (VisualControlContext visualControlContext = new VisualControlContext())
            {
                return visualControlContext.Defects.Where(x => x.WaferId == waferId).ToList();
            }
        }

        public List<Defect> GetByDieId(long dieId)
        {
            using (VisualControlContext visualControlContext = new VisualControlContext())
            {
                return visualControlContext.Defects.Where(x => x.DieId == dieId).ToList();
            }
        }

        public List<Defect> GetByWaferIdWithIncludes(string waferId)
        {
            using (VisualControlContext visualControlContext = new VisualControlContext())
            {
                return visualControlContext.Defects.Include(x=>x.DangerLevel).Include(x=>x.DefectType).Where(x => x.WaferId == waferId).ToList();
            }
        }
    }
}
