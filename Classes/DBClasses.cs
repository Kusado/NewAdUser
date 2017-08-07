using System;

namespace NewAdUser {

  public class Roles {

    #region Instance Properties

    public Guid IDRole { get; set; }
    public Guid IDDB { get; set; }
    public String RoleName { get; set; }
    public String RoleCaption { get; set; }
    public String RoleNote { get; set; }
    public DateTime CreateDate { get; set; }
    public Int32? IDAuthor { get; set; }
    public Int32? IDModifier { get; set; }
    public DateTime? ModifyDate { get; set; }
    public String ModifyLogin { get; set; }

    #endregion Instance Properties
  }

  public class KBMenu {

    #region Instance Properties

    public Byte? IdMenuType { get; set; }

    public Int32? IDAuthor { get; set; }

    public String MenuName { get; set; }

    public String Purpose { get; set; }

    public String Note { get; set; }

    public Boolean? Visible { get; set; }

    public Boolean? Enable { get; set; }

    public Boolean Sys { get; set; }

    public Guid IdMenu { get; set; }

    public DateTime CreateDate { get; set; }

    public Int32? IDModifier { get; set; }

    public DateTime? ModifyDate { get; set; }

    public String ModifyLogin { get; set; }

    public Guid IdIS { get; set; }

    public Byte IDMenuViewType { get; set; }

    public Guid IDMenuPicture { get; set; }

    public Guid IDMenuItemDefault { get; set; }

    public Boolean ShowFilterControl { get; set; }

    #endregion Instance Properties
  }
}