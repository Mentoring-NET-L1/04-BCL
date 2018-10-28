using System;
using System.IO;
using System.Text;
using FileDistributorService.Configuration;
using FileManagment;

namespace FileDistributorService
{
    internal class PathMapper
    {
        private MapRuleElement _mapRule;
        private int _fileNumber;

        public PathMapper(MapRuleElement mapRule)
        {
            _mapRule = mapRule;
        }

        public string Map(string filePath)
        {
            if (FileHelper.IsFileMatchPattern(filePath, _mapRule.FileNameRegex))
            {
                var fileName = Path.GetFileName(filePath);

                var nameBuilder = new StringBuilder();
                if (_mapRule.AddSerialNumber)
                {
                    nameBuilder.Append(_fileNumber++);
                    nameBuilder.Append(". ");
                }
                if (_mapRule.AddMoveDate)
                {
                    var date = DateTime.Now.ToString().Replace('/', '-').Replace(':', '-').Replace(' ', '_');
                    nameBuilder.Append(date);
                    nameBuilder.Append(" - ");
                }
                nameBuilder.Append(fileName);

                return Path.Combine(_mapRule.DestDirectory, nameBuilder.ToString());
            }

            return null;
        }
    }
}
