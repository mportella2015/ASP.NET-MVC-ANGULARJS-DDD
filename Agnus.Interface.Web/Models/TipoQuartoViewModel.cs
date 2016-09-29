using Agnus.Domain.Models;
using Agnus.Interface.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Agnus.Interface.Web.Models
{
    [Serializable]
    public class TipoQuartoViewModel : BaseViewModel<TipoQuartoViewModel>
    {

        public TipoQuartoViewModel()
        {

        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Descricao { get; set; }
        public string PossuiCamaCasal { get; set; }
        public string PossuiAR { get; set; }
        public string PossuiFrigoBar { get; set; }
        public string PossuiInternet { get; set; }
        public string PossuiHidroMassagem { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public int QuantidadePermitidaPessoas { get; set; }

        [StringLength(500)]
        public string Observacao { get; set; }

    }
}