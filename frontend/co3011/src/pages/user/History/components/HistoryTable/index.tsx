import { flexRender, type Table } from "@tanstack/react-table";

interface HistoryTableProps {
  customTable: Table<any>;
}

export function HistoryTable({ customTable }: HistoryTableProps) {
  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        gap: 10,
        marginTop: 20,
      }}
    >
      <table
        style={{
          borderTop: "1px solid #4d4d4d",
          borderLeft: "1px solid black",
        }}
      >
        <thead>
          {customTable.getHeaderGroups().map((headerGroup) => (
            <tr key={headerGroup.id}>
              {headerGroup.headers.map((header) => (
                <th
                  key={header.id}
                  colSpan={header.colSpan}
                  style={{
                    borderBottom: "1px solid #4d4d4d",
                    borderRight: "1px solid black",
                    padding: "2px 4px",
                  }}
                >
                  <div
                    {...{
                      className: "",
                      onClick: header.column.getToggleSortingHandler(),
                    }}
                  >
                    {flexRender(
                      header.column.columnDef.header,
                      header.getContext()
                    )}
                    {{
                      asc: " 🔼",
                      desc: " 🔽",
                    }[header.column.getIsSorted() as string] ?? null}
                  </div>
                </th>
              ))}
            </tr>
          ))}
        </thead>

        <tbody>
          {customTable.getRowModel().rows.map((row) => (
            <tr key={row.id}>
              {row.getVisibleCells().map((cell) => (
                <td
                  key={cell.id}
                  style={{
                    borderBottom: "1px solid #4d4d4d",
                    borderRight: "1px solid black",
                    padding: "2px 4px",
                    textAlign: "center",
                  }}
                >
                  {flexRender(cell.column.columnDef.cell, cell.getContext())}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
      <div
        style={{
          display: "flex",
          alignItems: "center",
          gap: 2,
          justifyContent: "space-between",
        }}
      >
        <div style={{ display: "flex", gap: 10 }}>
          <span className="flex" style={{ alignItems: "center", gap: 3 }}>
            Go to page:
            <input
              type="number"
              min="1"
              max={customTable.getPageCount()}
              defaultValue={customTable.getState().pagination.pageIndex + 1}
              onChange={(e) => {
                const page = e.target.value ? Number(e.target.value) - 1 : 0;
                customTable.setPageIndex(page);
              }}
              //   className="border p-1 rounded w-16"
              style={{
                border: "1px solid lightgray",
                padding: 1,
                borderRadius: 10,
              }}
            />
          </span>
          <select
            value={customTable.getState().pagination.pageSize}
            onChange={(e) => {
              customTable.setPageSize(Number(e.target.value));
            }}
            style={{
              border: "1px solid lightgray",
              padding: 1,
              borderRadius: 10,
            }}
          >
            {[10, 20, 30, 40, 50].map((pageSize) => (
              <option key={pageSize} value={pageSize}>
                Show {pageSize}
              </option>
            ))}
          </select>
        </div>
        <div style={{ display: "flex", gap: 10 }}>
          <button
            style={{ padding: 1 }}
            onClick={() => customTable.firstPage()}
            disabled={!customTable.getCanPreviousPage()}
          >
            {"|<"}
          </button>
          <button
            style={{ padding: 1 }}
            onClick={() => customTable.previousPage()}
            disabled={!customTable.getCanPreviousPage()}
          >
            {"<"}
          </button>
          <span>
            <strong>
              {customTable.getState().pagination.pageIndex + 1} ..
              {customTable.getPageCount().toLocaleString()}
            </strong>
          </span>
          <button
            style={{ padding: 1 }}
            onClick={() => customTable.nextPage()}
            disabled={!customTable.getCanNextPage()}
          >
            {">"}
          </button>
          <button
            style={{ padding: 1 }}
            onClick={() => customTable.lastPage()}
            disabled={!customTable.getCanNextPage()}
          >
            {">|"}
          </button>
        </div>
      </div>
    </div>
  );
}
