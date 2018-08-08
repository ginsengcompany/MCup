using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{



    public class Metadati
    {
        public string desDocumento { get; set; }
        public string desEvento { get; set; }
        public string dataDocumento { get; set; }
        public string autoreDocumento { get; set; }
        public string RefertoPiuData { get; set; }
    }

    public class ListaReferti
    {
        public Metadati metadati { get; set; }
        public string id { get; set; }
        public string filename { get; set; }
    }

    public class Referto
    {
        public List<ListaReferti> listaReferti { get; set; }
    }


}
/*{
    "listaReferti": [
        {
            "metadati": {
                "desDocumento": "REFERTO CONSULENZA",
                "desEvento": "ACCESSO AMBULATORIALE",
                "sender": "SOL",
                "userid": "214",
                "fileType": "pdf",
                "tipoFolder": "AMB",
                "progFolder": "2017000000",
                "tipoDocumento": "004.0001",
                "progDocumento": "000000000000000000000000",
                "dataDocumento": "25/05/2017",
                "autoreDocumento": "MEDICO XXX",
                "tipoEvento": "3",
                "progEvento": "2017051369",
                "codicestruttura": "150907",
                "codiceunitaoperativa": "4301",
                "codiceambulatorio": "2",
                "idanagrafica": "640150",
                "codicefiscale": "FCLMRZ62T18B963U",
                "cognome": "FUCILE",
                "nome": "MAURIZIO",
                "datanascita": "18/12/1962",
                "sesso": "M",
                "istatcomunenascita": "061022",
                "comunenascita": "CASERTA",
                "mimeType": "application/pdf"
            },
            "id": "5926e32733553a462d8e78ae",
            "filename": "AMB-2017000000-004.0001-000000000000000000000000-SOL.pdf"
        },
        {
            "metadati": {
                "desDocumento": "REFERTO DI LABORATORIO",
                "desEvento": "ACCESSO AMBULATORIALE",
                "sender": "SOL",
                "userid": "214",
                "fileType": "pdf",
                "tipoFolder": "AMB",
                "progFolder": "2017000000",
                "tipoDocumento": "003.0001",
                "progDocumento": "000000000000000000000001",
                "dataDocumento": "25/05/2017",
                "autoreDocumento": "MEDICO XXX",
                "tipoEvento": "3",
                "progEvento": "2017051369",
                "codicestruttura": "150907",
                "codiceunitaoperativa": "4301",
                "codiceambulatorio": "2",
                "idanagrafica": "640150",
                "codicefiscale": "FCLMRZ62T18B963U",
                "cognome": "FUCILE",
                "nome": "MAURIZIO",
                "datanascita": "18/12/1962",
                "sesso": "M",
                "istatcomunenascita": "061022",
                "comunenascita": "CASERTA",
                "mimeType": "application/pdf"
            },
            "id": "5926e3f233553a462d8e78b1",
            "filename": "AMB-2017000000-003.0001-000000000000000000000001-SOL.pdf"
        },
        {
            "metadati": {
                "desDocumento": "REFERTO DI RADIOLOGIA",
                "desEvento": "ACCESSO AMBULATORIALE",
                "sender": "SOL",
                "userid": "214",
                "fileType": "pdf",
                "tipoFolder": "AMB",
                "progFolder": "2017000000",
                "tipoDocumento": "002.0001",
                "progDocumento": "000000000000000000000002",
                "dataDocumento": "25/05/2017",
                "autoreDocumento": "MEDICO XXX",
                "tipoEvento": "3",
                "progEvento": "2017051369",
                "codicestruttura": "150907",
                "codiceunitaoperativa": "4301",
                "codiceambulatorio": "2",
                "idanagrafica": "640150",
                "codicefiscale": "FCLMRZ62T18B963U",
                "cognome": "FUCILE",
                "nome": "MAURIZIO",
                "datanascita": "18/12/1962",
                "sesso": "M",
                "istatcomunenascita": "061022",
                "comunenascita": "CASERTA",
                "mimeType": "application/pdf"
            },
            "id": "5926e5f933553a462d8e78b5",
            "filename": "AMB-2017000000-002.0001-000000000000000000000002-SOL.pdf"
        },
        {
            "metadati": {
                "desDocumento": "CARTELLA AMBULATORIALE",
                "desEvento": "ACCESSO AMBULATORIALE",
                "sender": "SOL",
                "userid": "214",
                "fileType": "pdf",
                "tipoFolder": "AMB",
                "progFolder": "2017000000",
                "tipoDocumento": "001.0003",
                "progDocumento": "000000000000000000000003",
                "dataDocumento": "25/05/2017",
                "autoreDocumento": "MEDICO XXX",
                "tipoEvento": "3",
                "progEvento": "2017051369",
                "codicestruttura": "150907",
                "codiceunitaoperativa": "4301",
                "codiceambulatorio": "2",
                "idanagrafica": "640150",
                "codicefiscale": "FCLMRZ62T18B963U",
                "cognome": "FUCILE",
                "nome": "MAURIZIO",
                "datanascita": "18/12/1962",
                "sesso": "M",
                "istatcomunenascita": "061022",
                "comunenascita": "CASERTA",
                "mimeType": "application/pdf"
            },
            "id": "5926e75133553a462d8e78b9",
            "filename": "AMB-2017000000-001.0003-000000000000000000000003-SOL.pdf"
        },
        {
            "metadati": {
                "desDocumento": "PROFILO SANITARIO SINT.",
                "desEvento": "ACCESSO AMBULATORIALE",
                "sender": "SOL",
                "userid": "214",
                "fileType": "pdf",
                "tipoFolder": "AMB",
                "progFolder": "2017000000",
                "tipoDocumento": "010.0002",
                "progDocumento": "000000000000000000000004",
                "dataDocumento": "25/05/2017",
                "autoreDocumento": "MEDICO BIANCHI",
                "tipoEvento": "3",
                "progEvento": "2017051369",
                "codicestruttura": "150907",
                "codiceunitaoperativa": "4301",
                "codiceambulatorio": "2",
                "idanagrafica": "640150",
                "codicefiscale": "FCLMRZ62T18B963U",
                "cognome": "FUCILE",
                "nome": "MAURIZIO",
                "datanascita": "18/12/1962",
                "sesso": "M",
                "istatcomunenascita": "061022",
                "comunenascita": "CASERTA",
                "mimeType": "application/pdf"
            },
            "id": "592d7f5b6d985f2842c31397",
            "filename": "AMB-2017000000-010.0002-000000000000000000000004-SOL.pdf"
        }
    ]
}
*/