using System;
using System.Collections.Generic;

namespace Seabites.Naughty.Security {
  public static class SecurityPermissions {
    public static readonly PermissionId AddPermissionToRole = new PermissionId(new Guid("5E8B9F65-5AB8-48ED-AF60-4D378E08C71B"));
    public static readonly PermissionId AddRole = new PermissionId(new Guid("FCFA3DA1-FCE6-48BF-883A-3D945558F5B5"));
    public static readonly PermissionId AddRoleGroup = new PermissionId(new Guid("0BD1FA27-9578-466C-B7AF-EF6DEAC63E9A"));
    public static readonly PermissionId AddRoleToRoleGroup = new PermissionId(new Guid("4232319F-26FC-4B45-A24D-797FF1FE04D0"));
    public static readonly PermissionId AddUserAccount = new PermissionId(new Guid("9721897A-FF61-46E2-8AE3-4DC1C943C294"));
    public static readonly PermissionId RemovePermissionFromRole = new PermissionId(new Guid("F23EC75C-8B82-4A2C-94A5-B8CBE064DB75"));
    public static readonly PermissionId ArchivedRole = new PermissionId(new Guid("892566C5-83C7-4117-9BB7-772898A598B4"));
    public static readonly PermissionId RemoveRoleFromRoleGroup = new PermissionId(new Guid("02081A24-7749-4ED6-AA98-FC89E42C894C"));
    public static readonly PermissionId ArchivedRoleGroup = new PermissionId(new Guid("A2425351-84E5-47F0-A359-174EE69C6429"));
    public static readonly PermissionId DisableUserAccount = new PermissionId(new Guid("149A9484-0F8A-4A37-B082-9D12E3B06ED6"));
    public static readonly PermissionId GrantRoleToUser = new PermissionId(new Guid("BCD4B8D9-512D-4892-95FD-9054025CAB72"));
    public static readonly PermissionId GrantRoleGroupToUser = new PermissionId(new Guid("14ECC237-674A-408A-BE98-0D1B9E936FA9"));
    public static readonly PermissionId RevokeRoleFromUser = new PermissionId(new Guid("4A5F9B1F-79DA-4282-A76C-F24FA2606786"));
    public static readonly PermissionId RevokeRoleGroupFromUser = new PermissionId(new Guid("C3447C12-1D55-4227-BBFE-EC925317FA50"));
    public static readonly PermissionId AllowRolePermission = new PermissionId(new Guid("58B2A4C5-34F9-405A-8035-4AFC94CF4F88"));
    public static readonly PermissionId DenyRolePermission = new PermissionId(new Guid("EEB19EDE-6C39-4DCA-BD1A-76017C327E2F"));

    public static IEnumerable<PermissionId> All = new [] {
      AddPermissionToRole,
      AddRole,
      AddRoleGroup,
      AddRoleToRoleGroup,
      AddUserAccount,
      RemovePermissionFromRole,
      ArchivedRole,
      RemoveRoleFromRoleGroup, 
      ArchivedRoleGroup,
      DisableUserAccount,
      GrantRoleToUser,
      GrantRoleGroupToUser,
      RevokeRoleFromUser,
      RevokeRoleGroupFromUser, 
      AllowRolePermission,
      DenyRolePermission
    };
  }
}