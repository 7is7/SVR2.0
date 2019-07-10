using System.Collections.Generic;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Providers
{
    public interface IMaterialProvider
    {
        Material ChangeName(string oldName, string newName);
        List<MaterialViewModel> GetAll();
        Material ChangeMaterialOnMeasurement(int measurementId, int materialId);
    } 
    
}