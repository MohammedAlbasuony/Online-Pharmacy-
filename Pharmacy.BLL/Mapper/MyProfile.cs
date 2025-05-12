using AutoMapper;
using Hospital_Mangment_System_BLL.View_model.patientVM;
using Pharmacy.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.BLL.Mapper
{
    public class MyProfile : Profile // Inherit from AutoMapper's Profile class
    {
        public MyProfile()
        {
            // Patient mappings
            CreateMap<CreatePatientVM, Patient>();

            CreateMap<CreatePatientVM, DAL.Entity.Patient>();
            CreateMap<DAL.Entity.Patient, GetAllPatientssVM>(); // Fixed typo in GetAllPatientsVM
            CreateMap<DAL.Entity.Patient, GetPatientByIdVM>();
            CreateMap<UpdatePatientVM, DAL.Entity.Patient>();
        }
    }
}
