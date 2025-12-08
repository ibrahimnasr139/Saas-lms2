using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Domain.Entites;
namespace Application.Features.Plan.DTOs
{
    public sealed class PlanProfile : Profile
    {
        public PlanProfile()
        {
            CreateMap<Domain.Entites.Plan, PlanResponse>()
             .ForMember(dest => dest.PlanPricings, opt => opt.MapFrom(src => src.PlanPricings))
             .ForMember(dest => dest.PlanFeatures, opt => opt.MapFrom(src => src.PlanFeatures));

            CreateMap<PlanPricing, PlanPricingResponse>()
                .ForMember(dest => dest.BillingCycle, opt => opt.MapFrom(src => src.BillingCycle.ToString()))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.DiscountPercent));

            CreateMap<PlanFeature, PlanFeatureResponse>()
                .ForMember(dest => dest.FeatureName, opt => opt.MapFrom(src => src.Feature.Name))
                .ForMember(dest => dest.FeatureDescription, opt => opt.MapFrom(src => src.Feature.Description))
                .ForMember(dest => dest.FeatureKey, opt => opt.MapFrom(src => src.Feature.Key));
        }
    }
}
