namespace Pry_TitulacionMovil.Helpers
{
    using Models;
    using System.Collections.Generic;

    public static class Converter
    {
        public static UserLocal ToUserLocal(User _user)
        {
            return new UserLocal
            {
                UserId = _user.UserId,
                UserName = _user.UserName,
                UserLastName = _user.UserLastName,
                Password = _user.Password,
                ImagenPath = string.IsNullOrEmpty(_user.ImagenPath) ? "ic_nouser" : _user.ImagenPath
            };
        }
        public static OrdenCabeDeta ToOrdenCabeDeta(Orden ordencabecera, List<OrdenDetalles> ordendetalle)
        {
            return new OrdenCabeDeta
            {
                IdOrden = ordencabecera.IdOrden,
                IdEquipo = ordencabecera.IdEquipo,
                Equipo = ordencabecera.Equipo,
                MarcaId = ordencabecera.MarcaId,
                ModeloId = ordencabecera.ModeloId,
                Voltaje = ordencabecera.Voltaje,
                Amperaje = ordencabecera.Amperaje,
                Presion = ordencabecera.Presion,
                FechaInicioTR = ordencabecera.FechaInicioTR,
                FechaFinalTR = ordencabecera.FechaFinalTR,
                Observacion = ordencabecera.Observacion,
                ImagenTR = ordencabecera.ImagenTR,
                RutaImagen = ordencabecera.RutaImagen,
                Latitud = ordencabecera.Latitud,
                Logintud = ordencabecera.Logintud,
                OrdenDetalles = ordendetalle
            };
        }
    }
}
