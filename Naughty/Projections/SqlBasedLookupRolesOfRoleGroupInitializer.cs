using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Seabites.Naughty.Projections {
  public class SqlBasedLookupRolesOfRoleGroupInitializer {
     readonly SqlConnectionStringBuilder _builder;

    public SqlBasedLookupRolesOfRoleGroupInitializer(SqlConnectionStringBuilder builder) {
      if (builder == null) throw new ArgumentNullException("builder");
      _builder = builder;
    }

    public ILookupRolesOfRoleGroup Initialize() {
      using (var connection = new SqlConnection(_builder.ConnectionString)) {
        connection.Open();
        var storage = new Dictionary<Guid, HashSet<Guid>>();
        LoadRoleGroups(connection, storage);
        LoadRolesInEachRoleGroup(connection, storage);
        return new MemoryRoleGroupLookup(storage);
      }
    }

    static void LoadRoleGroups(SqlConnection connection, Dictionary<Guid, HashSet<Guid>> storage) {
      using (var command = new SqlCommand("SELECT DISTINCT(RoleGroupId) FROM UserAccountEffectiveRoles WHERE RoleGroupId IS NOT NULL", connection)) {
        using (var reader = command.ExecuteReader()) {
          if (!reader.IsClosed) {
            while (reader.Read()) {
              storage.Add(reader.GetGuid(0), new HashSet<Guid>());
            }
            reader.Close();
          }
        }
      }
    }

    static void LoadRolesInEachRoleGroup(SqlConnection connection, Dictionary<Guid, HashSet<Guid>> storage) {
      using (
        var command =
          new SqlCommand(
            "SELECT DISTINCT(RoleId) FROM UserAccountEffectiveRoles WHERE RoleGroupId = @RoleGroupId AND RoleId IS NOT NULL",
            connection)) {
        command.Parameters.Add(new SqlParameter("@RoleGroupId", default(Guid)));
        foreach (var roleGroupId in storage.Keys) {
          command.Parameters["@RoleGroupId"].Value = roleGroupId;
          var roles = storage[roleGroupId];
          using (var reader = command.ExecuteReader()) {
            if (!reader.IsClosed) {
              while (reader.Read()) {
                roles.Add(reader.GetGuid(0));
              }
              reader.Close();
            }
          }
        }
      }
    }
  }
}
