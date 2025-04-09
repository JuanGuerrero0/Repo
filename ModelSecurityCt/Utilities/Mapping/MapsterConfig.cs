using Entity.DTO;
using Entity.Model;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Mapping
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Rol, RolDTO>.NewConfig();
            TypeAdapterConfig<RolDTO, Rol>.NewConfig();
            TypeAdapterConfig<User, UserDTO>.NewConfig();
            TypeAdapterConfig<UserDTO, User>.NewConfig();
            TypeAdapterConfig<Permission, PermissionDTO>.NewConfig();
            TypeAdapterConfig<PermissionDTO, Permission>.NewConfig();
            TypeAdapterConfig<Module, ModuleDTO>.NewConfig();
            TypeAdapterConfig<ModuleDTO, Module>.NewConfig();
            TypeAdapterConfig<Person, PersonDTO>.NewConfig();
            TypeAdapterConfig<PersonDTO, Person>.NewConfig();
            TypeAdapterConfig<RolUser, RolUserDTO>.NewConfig();
            TypeAdapterConfig<RolUserDTO, RolUser>.NewConfig();
            TypeAdapterConfig<Form, FormDTO>.NewConfig();
            TypeAdapterConfig<FormDTO, Form>.NewConfig();
            TypeAdapterConfig<RolFormPermissionDTO, RolFormPermission>.NewConfig();
            TypeAdapterConfig<RolFormPermission, RolFormPermissionDTO>.NewConfig();
            TypeAdapterConfig<FormModuleDTO, FormModule>.NewConfig();
            TypeAdapterConfig<FormModule, FormModuleDTO>.NewConfig();
            // Agrega más mapeos aquí si lo necesitas
        }
    }
}