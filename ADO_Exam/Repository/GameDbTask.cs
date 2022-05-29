using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADO_Exam.Repository
{
    public class GameDbTask
    {
        public delegate IEnumerable<object>QueryDelegate(string arg);

        public string TaskName { get; set; } = String.Empty;
        public QueryDelegate Query { get; set; } = null;
        public bool HasParameter { get; set; } = false;

        public IEnumerable<object> Run(string arg = null) => Query.Invoke(arg);
        public Task<IEnumerable<object>> RunAsync(string arg = null) => Task.Run(() => Run(arg));

        public override string ToString() => TaskName;
    }
}
