import {
  getCoreRowModel,
  getFilteredRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  useReactTable,
  type ColumnDef,
  type PaginationState,
} from "@tanstack/react-table";
import { useMemo, useState } from "react";
import { HistoryTable } from "../HistoryTable";

export type BookQueueType = {
  name: string;
  author: string;
  borrowDate: string;
  returnDate: string;
  dueDate: string;
  status: string;
};

const mockData: BookQueueType[] = [
  {
    name: "The Gun of August",
    author: "Barbara W. Tuchman",
    borrowDate: "28/09/2025",
    returnDate: "N/A",
    dueDate: "28/10/2025",
    status: "Pending",
  },
  {
    name: "Im Westen nichts Neuest",
    author: "Erich Maria Remarque",
    borrowDate: "14/08/2025",
    returnDate: "14/09/2025",
    dueDate: "14/10/2025",
    status: "On Time",
  },
  {
    name: "A Farewell to Arms",
    author: "Erich Hemingway",
    borrowDate: "11/07/2025",
    returnDate: "11/08/2025",
    dueDate: "11/09/2025",
    status: "On Time",
  },
  {
    name: "The Sleepwalkers: How Europe Went to War in 1914",
    author: "Christopher Clark, Paul Grossman",
    borrowDate: "06/06/2025",
    returnDate: "06/07/2025",
    dueDate: "06/08/2025",
    status: "On Time",
  },
  {
    name: "Catastrophe 1914: Europe Goes to War",
    author: "Max Hugh Macdonald Hastings",
    borrowDate: "20/05/2025",
    returnDate: "20/06/2025",
    dueDate: "20/07/2025",
    status: "On Time",
  },
  {
    name: "Germany's Aims in the First World War",
    author: "Fritz Fischer",
    borrowDate: "01/04/2025",
    returnDate: "01/05/2025",
    dueDate: "01/06/2025",
    status: "On Time",
  },
  {
    name: "The Art of War",
    author: "Sun Tzu",
    borrowDate: "12/03/2025",
    returnDate: "12/06/2025",
    dueDate: "12/05/2025",
    status: "Late",
  },
  {
    name: "The Rise and Fall of the Third Reich",
    author: "William L. Shirer",
    borrowDate: "28/02/2025",
    returnDate: "29/04/2025",
    dueDate: "28/04/2025",
    status: "Late",
  },
  {
    name: "The Prince",
    author: "Niccolò Machiavelli",
    borrowDate: "01/01/2025",
    returnDate: "01/04/2025",
    dueDate: "01/03/2025",
    status: "Late",
  },
  {
    name: "The Rise and Fall of the Third Reich",
    author: "William L. Shirer",
    borrowDate: "11/09/2025",
    returnDate: "07/10/2025",
    dueDate: "11/11/2025",
    status: "On Time",
  },
  {
    name: "The Rise and Fall of the Third Reich",
    author: "William L. Shirer",
    borrowDate: "11/09/2025",
    returnDate: "07/10/2025",
    dueDate: "11/11/2025",
    status: "On Time",
  },
];

export function BookQueue() {
  const columns = useMemo<ColumnDef<BookQueueType>[]>(
    () => [
      {
        accessorKey: "name",
        cell: (info) => info.getValue(),
        header: "Name",
        footer: (props) => props.column.id,
      },
      {
        accessorKey: "author",
        cell: (info) => info.getValue(),
        header: "Author",
        footer: (props) => props.column.id,
      },
      {
        accessorKey: "borrowDate",
        cell: (info) => info.getValue(),
        header: "Borrow Date",
        footer: (props) => props.column.id,
      },
      {
        accessorKey: "returnDate",
        cell: (info) => info.getValue(),
        header: "Return Date",
        footer: (props) => props.column.id,
      },
      {
        accessorKey: "dueDate",
        cell: (info) => info.getValue(),
        header: "Due Date",
        footer: (props) => props.column.id,
      },
      {
        accessorKey: "status",
        cell: (info) => info.getValue(),
        header: "Status",
        footer: (props) => props.column.id,
      },
    ],
    []
  );

  const [pagination, setPagination] = useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  });

  const customTable = useReactTable({
    columns,
    data: mockData,
    debugTable: true,
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    onPaginationChange: setPagination,
    state: {
      pagination,
    },
  });

  return <HistoryTable customTable={customTable} />;
}
