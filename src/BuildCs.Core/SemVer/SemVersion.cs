using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BuildCs.SemVer
{
    public class SemVersion : IEquatable<SemVersion>
    {
        private static readonly Regex _regex = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)(-(?<prerelease>[A-Za-z0-9\-\.]+))?(\+(?<build>[A-Za-z0-9\-\.]+))?", RegexOptions.Compiled | RegexOptions.Singleline);

        public SemVersion(string versionString)
        {
            var match = _regex.Match(versionString);
            if (!match.Success)
                throw new BuildCsException("Invalid SemVer: {0}".F(versionString));

            Major = int.Parse(match.Groups["major"].Value, CultureInfo.InvariantCulture);
            Minor = int.Parse(match.Groups["minor"].Value, CultureInfo.InvariantCulture);
            Patch = int.Parse(match.Groups["patch"].Value, CultureInfo.InvariantCulture);

            var preReleaseGroup = match.Groups["prerelease"];
            if (preReleaseGroup.Success)
                PreRelease = preReleaseGroup.Value;

            var buildMetadataGroup = match.Groups["build"];
            if (buildMetadataGroup.Success)
                BuildMetadata = buildMetadataGroup.Value;
        }

        public SemVersion(int major, int minor, int patch, string preRelease = null, string buildMetadata = null)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            PreRelease = preRelease;
            BuildMetadata = buildMetadata;
        }

        public int Major { get; private set; }

        public int Minor { get; private set; }

        public int Patch { get; private set; }

        public string PreRelease { get; private set; }

        public string BuildMetadata { get; private set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as SemVersion);
        }

        public bool Equals(SemVersion other)
        {
            if (other == null)
                return false;

            return Major == other.Major
                && Minor == other.Minor
                && Patch == other.Patch
                && PreRelease == other.PreRelease;
        }

        public override int GetHashCode()
        {
            return Major.GetHashCode()
                ^ Minor.GetHashCode()
                ^ Patch.GetHashCode()
                ^ (PreRelease ?? "").GetHashCode();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}.{1}.{2}", Major, Minor, Patch);
            if (!string.IsNullOrWhiteSpace(PreRelease))
                sb.AppendFormat("-{0}", PreRelease);
            if (!string.IsNullOrWhiteSpace(BuildMetadata))
                sb.AppendFormat("+{0}", BuildMetadata);
            return sb.ToString();
        }

        public Version ToVersion(int revision)
        {
            return new Version(Major, Minor, Patch, revision);
        }

        public static implicit operator string(SemVersion semVersion)
        {
            return semVersion.ToString();
        }

        public static implicit operator SemVersion(string versionString)
        {
            return new SemVersion(versionString);
        }
    }
}