using System.Threading.Tasks;
using VueExample.Contexts;
using VueExample.Entities;
using VueExample.Providers.Abstract;

namespace VueExample.Providers
{
public class KurbatovParameterProvider : IKurbatovParameterProvider
    {
        private readonly Srv6Context _srv6Context;
        public KurbatovParameterProvider(Srv6Context srv6Context)
        {
            _srv6Context = srv6Context;
        }
        public async Task<KurbatovParameterEntity> Create(int? bordersId, int standartParameterId, int standartMeasurementPatternId)
        {
            var kurbatovParameterEntity = new KurbatovParameterEntity{BordersId = bordersId, StandartParameterId = standartParameterId, SmpId = standartMeasurementPatternId};
            _srv6Context.KurbatovParameters.Add(kurbatovParameterEntity);
            await _srv6Context.SaveChangesAsync();
            return kurbatovParameterEntity;
        }
    }
}