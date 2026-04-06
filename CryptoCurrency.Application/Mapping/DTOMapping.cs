using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.Mapping
{
    public class DTOMapping :Profile
    {
        public DTOMapping() {

            //CreateMap<Employee, LoginResDTO>().ForMember(x => x.RoleName, x => x.MapFrom(x => x.AddRole.RoleName));

            //// Employee Mapping

            //CreateMap<Employee, EmployeeDTO>().ReverseMap();

            //CreateMap<Employee, EmpFeatchDTO>()
            // .ForMember(x => x.DepartmentName,
            //    x => x.MapFrom(x => x.AddDepartments.DepartmentName != null ? x.AddDepartments.DepartmentName : "Department Not Exist")
            //    )
            //.ForMember(x => x.DesignationName,
            //    x => x.MapFrom(x => x.AddDesignation.DesignationName != null ? x.AddDesignation.DesignationName : "Designation Not Exist"))
            //.ForMember(x => x.RoleName,
            //    x => x.MapFrom(x => x.AddRole.RoleName != null ? x.AddRole.RoleName : "Role Not Exist"));
        }
    }
}
