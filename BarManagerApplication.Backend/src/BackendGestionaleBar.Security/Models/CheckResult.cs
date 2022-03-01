namespace BackendGestionaleBar.Security.Models
{
    public class CheckResult
    {
        public CheckResult(bool verified, bool needsUpgrade)
        {
            Verified = verified;
            NeedsUpgrade = needsUpgrade;
        }

        public bool Verified { get; }
        public bool NeedsUpgrade { get; }
    }
}