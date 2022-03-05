namespace BackendGestionaleBar.Security.Models.Response
{
    public sealed class CheckPasswordResponse
    {
        internal CheckPasswordResponse(bool verified, bool needsUpgrade)
        {
            Verified = verified;
            NeedsUpgrade = needsUpgrade;
        }

        public bool Verified { get; }
        public bool NeedsUpgrade { get; }
    }
}