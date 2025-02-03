using System.Xml.Serialization;

[XmlRoot("Customers")]
public class Customers
{
    [XmlElement("Customer")]
    public List<Customer> CustomerList { get; set; }
}

public class Customer
{
    public Customer()
    {
        Addresses = new List<Address>();
    }

    [XmlArray("Addresses")]
    [XmlArrayItem("Address")]
    public List<Address> Addresses { get; set; }

    [XmlElement("Fields")]
    public Fields Fields { get; set; }

    [XmlElement("FieldTemplateSystemId")]
    public string FieldTemplateSystemId { get; set; }

    [XmlElement("Id")]
    public string Id { get; set; }

    [XmlElement("SystemId")]
    public string SystemId { get; set; }

    [XmlElement("Organization")]
    public Organization Organization { get; set; }
}

public class Address
{
    [XmlElement("AddressTypeSystemId")]
    public string AddressTypeSystemId { get; set; }

    [XmlElement("SystemId")]
    public string SystemId { get; set; }

    [XmlElement("Address1")]
    public string Address1 { get; set; }

    [XmlElement("Address2")]
    public string Address2 { get; set; }

    [XmlElement("CareOf")]
    public string CareOf { get; set; }

    [XmlElement("City")]
    public string City { get; set; }

    [XmlElement("Country")]
    public string Country { get; set; }

    [XmlElement("HouseExtension")]
    public string HouseExtension { get; set; }

    [XmlElement("HouseNumber")]
    public string HouseNumber { get; set; }

    [XmlElement("State")]
    public string State { get; set; }

    [XmlElement("ZipCode")]
    public string ZipCode { get; set; }

    [XmlElement("PhoneNumber")]
    public string PhoneNumber { get; set; }
}

public class Fields
{
    [XmlElement("Name")]
    public Name Name { get; set; }

    [XmlElement("NameInvariantCulture")]
    public string NameInvariantCulture { get; set; }

    [XmlElement("Description")]
    public Description Description { get; set; }

    [XmlElement("Email")]
    public string Email { get; set; }

    [XmlElement("FirstName")]
    public string FirstName { get; set; }

    [XmlElement("LastName")]
    public string LastName { get; set; }

    [XmlElement("PhoneNumber")]
    public string PhoneNumber { get; set; }

    [XmlElement("Title")]
    public string Title { get; set; }

    [XmlElement("Option")]
    public Option Option { get; set; }

    [XmlElement("MatchCondition")]
    public string MatchCondition { get; set; }

    [XmlElement("LegalRegistrationNumber")]
    public string LegalRegistrationNumber { get; set; }

    [XmlElement("SocialSecurityNumber")]
    public string SocialSecurityNumber { get; set; }
}

public class Name
{
    [XmlElement("EnUS")]
    public string EnUS { get; set; }

    [XmlElement("SvSE")]
    public string SvSE { get; set; }
}

public class Description
{
    [XmlElement("EnUS")]
    public string EnUS { get; set; }

    [XmlElement("SvSE")]
    public string SvSE { get; set; }
}

public class Option
{
    [XmlElement("Subscription")]
    public string Subscription { get; set; }
}

public class Organization
{
    [XmlElement("LoginCredential")]
    public LoginCredential LoginCredential { get; set; }

    [XmlArray("Addresses")]
    [XmlArrayItem("Address")]
    public List<Address> Addresses { get; set; }

    [XmlElement("Fields")]
    public Fields Fields { get; set; }

    [XmlElement("FieldTemplateSystemId")]
    public string FieldTemplateSystemId { get; set; }

    [XmlElement("GroupLinks")]
    public GroupLinks GroupLinks { get; set; }

    [XmlElement("Id")]
    public string Id { get; set; }

    [XmlElement("SystemId")]
    public string SystemId { get; set; }
}

public class LoginCredential
{
    [XmlElement("NewPassword")]
    public string NewPassword { get; set; }

    [XmlElement("LockoutEnabled")]
    public bool LockoutEnabled { get; set; }

    [XmlElement("LockoutEndDate")]
    public DateTime LockoutEndDate { get; set; }

    [XmlElement("PasswordExpirationDate")]
    public DateTime PasswordExpirationDate { get; set; }

    [XmlElement("Username")]
    public string Username { get; set; }
}

public class GroupLinks
{
    [XmlArrayItem("GroupLink")]
    public List<GroupLink> GroupLinksList { get; set; }
}

public class GroupLink
{
    [XmlElement("GroupSystemId")]
    public string GroupSystemId { get; set; }
}
