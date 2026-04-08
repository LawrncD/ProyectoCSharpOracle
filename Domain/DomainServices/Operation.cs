namespace ProyectoCSharpOracle.Domain.DomainServices
{
    /// <summary>
    /// Enum que representa las operaciones permitidas en el sistema.
    /// </summary>
    public enum Operation
    {
        // Consultas permitidas para Esporadico
        QueryExpensivePlayerByConfederation,
        QueryMatchesInStadium,
        QueryMostExpensiveTeamByCountry,
        QueryYoungPlayersByTeam,

        // Operaciones generales (Equipos, Confederaciones, etc.)
        ManageTeams,
        ManageConfederations,
        ManagePlayers,
        ManageMatches,

        // Solo Admin
        AddUser,
        ManageUsers,

        // Registro de salida (no para Esporadico)
        RegisterExit
    }
}