
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace web_server.Model
{

using System;
    using System.Collections.Generic;
    
public partial class ServiceChoosed
{

    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int ProviderId { get; set; }

    public int ServiceId { get; set; }

    public bool Accepted { get; set; }

    public string CustomerNote { get; set; }

    public bool FinishedByProvider { get; set; }

    public bool FinishedByCustomer { get; set; }



    public virtual Customer Customer { get; set; }

    public virtual Provider Provider { get; set; }

    public virtual Service Service { get; set; }

    public virtual Rate Rate { get; set; }

}

}