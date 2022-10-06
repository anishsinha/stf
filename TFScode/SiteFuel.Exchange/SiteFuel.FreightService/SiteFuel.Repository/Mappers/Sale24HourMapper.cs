using SiteFuel.FreightModels.ForcastingHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFill.DemandCaptureDataAccess.Entities;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class Sale24HourMapper
    {
        public static Sale24Hours ToEntity(this ISale24HourModel model)
        {
            var entity = new Sale24Hours();
            entity.Id = model.Id;
            entity.Date = model.Date;
            //entity.SaleTankId = model.SaleTank
            entity.From0To1 = model.From00To01;
            entity.From1To2 = model.From01To02;
            entity.From2To3 = model.From02To03;
            entity.From3To4 = model.From03To04;
            entity.From4To5 = model.From04To05;
            entity.From5To6 = model.From05To06;
            entity.From6To7 = model.From06To07;
            entity.From7To8 = model.From07To08;
            entity.From8To9 = model.From08To09;
            entity.From9To10 = model.From09To10;
            entity.From10To11 = model.From10To11;
            entity.From11To12 = model.From11To12;
            entity.From12To13 = model.From12To13;
            entity.From13To14 = model.From13To14;
            entity.From14To15 = model.From14To15;
            entity.From15To16 = model.From15To16;
            entity.From16To17 = model.From16To17;
            entity.From17To18 = model.From17To18;
            entity.From18To19 = model.From18To19;
            entity.From19To20 = model.From19To20;
            entity.From20To21 = model.From20To21;
            entity.From21To22 = model.From21To22;
            entity.From22To23 = model.From22To23;
            entity.From23To0 = model.From23To00;
            return entity;
        }

        public static void UpdateValues(this Sale24Hours entity, ISale24HourModel model)
        {
            entity.Date = model.Date;
            //entity.SaleTankId = model.SaleTankId;
            entity.From0To1 = model.From00To01;
            entity.From1To2 = model.From01To02;
            entity.From2To3 = model.From02To03;
            entity.From3To4 = model.From03To04;
            entity.From4To5 = model.From04To05;
            entity.From5To6 = model.From05To06;
            entity.From6To7 = model.From06To07;
            entity.From7To8 = model.From07To08;
            entity.From8To9 = model.From08To09;
            entity.From9To10 = model.From09To10;
            entity.From10To11 = model.From10To11;
            entity.From11To12 = model.From11To12;
            entity.From12To13 = model.From12To13;
            entity.From13To14 = model.From13To14;
            entity.From14To15 = model.From14To15;
            entity.From15To16 = model.From15To16;
            entity.From16To17 = model.From16To17;
            entity.From17To18 = model.From17To18;
            entity.From18To19 = model.From18To19;
            entity.From19To20 = model.From19To20;
            entity.From20To21 = model.From20To21;
            entity.From21To22 = model.From21To22;
            entity.From22To23 = model.From22To23;
            entity.From23To0 = model.From23To00;
        }

        public static Sale24HourModel ToViewModel(this Sale24Hours entity)
        {
            var model = new Sale24HourModel();
            model.Id = entity.Id;
            model.Date = entity.Date;
            model.SaleTankId = entity.SaleTankId;
            model.SaleTank = entity.SaleTank.ToViewModel();
            model.From00To01 = entity.From0To1;
            model.From01To02 = entity.From1To2;
            model.From02To03 = entity.From2To3;
            model.From03To04 = entity.From3To4;
            model.From04To05 = entity.From4To5;
            model.From05To06 = entity.From5To6;
            model.From06To07 = entity.From6To7;
            model.From07To08 = entity.From7To8;
            model.From08To09 = entity.From8To9;
            model.From09To10 = entity.From9To10;
            model.From10To11 = entity.From10To11;
            model.From11To12 = entity.From11To12;
            model.From12To13 = entity.From12To13;
            model.From13To14 = entity.From13To14;
            model.From14To15 = entity.From14To15;
            model.From15To16 = entity.From15To16;
            model.From16To17 = entity.From16To17;
            model.From17To18 = entity.From17To18;
            model.From18To19 = entity.From18To19;
            model.From19To20 = entity.From19To20;
            model.From20To21 = entity.From20To21;
            model.From21To22 = entity.From21To22;
            model.From22To23 = entity.From22To23;
            model.From23To00 = entity.From23To0;
            return model;
        }
    }
}
