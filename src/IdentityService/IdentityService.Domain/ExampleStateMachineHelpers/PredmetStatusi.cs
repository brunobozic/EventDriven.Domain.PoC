namespace IdentityService.Domain.ExampleStateMachineHelpers;
//public class PredmetStatusi : DomainEnumeration
//{
//    //public static StatusPredmeta Draft = new StatusPredmeta(1);
//    //public static StatusPredmeta Priprema = new StatusPredmeta(2);
//    //public static StatusPredmeta Otvoren = new StatusPredmeta(3);
//    //public static StatusPredmeta Zatvoren = new StatusPredmeta(4);
//    //public static StatusPredmeta Arhiviran = new StatusPredmeta(5);

//    //public PredmetStatusi(int id, string name)
//    //    : base(id, name)
//    //{
//    //}

//    //public static IEnumerable<StatusPredmeta> List() =>
//    //    new[] { Draft, Priprema, Otvoren, Zatvoren, Arhiviran };

//    //public static StatusPredmeta FromName(string name)
//    //{
//    //    var state = List()
//    //        .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

//    //    if (state == null)
//    //    {
//    //        throw new DomainException($"Possible values for StatusPredmeta: {String.Join(",", List().Select(s => s.Name))}");
//    //    }

//    //    return state;
//    //}

//    //public static StatusPredmeta From(int id)
//    //{
//    //    var state = List().SingleOrDefault(s => s.StatusPredmetaID == id);

//    //    if (state == null)
//    //    {
//    //        throw new DomainException($"Possible values for StatusPredmeta: {String.Join(",", List().Select(s => s.Name))}");
//    //    }

//    //    return state;
//    //}
//}