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

export type PointType = {
  id: number;
  time: string;
  pointBefore: number;
  pointAfter: number;
  changePoint: string;
  reasons: string;
};

const mockData: PointType[] = [
  {
    id: 972,
    time: "9:30 14/09/2025",
    pointBefore: 30682,
    pointAfter: 30712,
    changePoint: "+30",
    reasons: "Returned Book: Im Westen nichts Neues in time",
  },
  {
    id: 971,
    time: "14:00 11/09/2025",
    pointBefore: 30082,
    pointAfter: 30682,
    changePoint: "+600",
    reasons: "Donated Book: Quantum Field Theory in a Nutshell",
  },
  {
    id: 970,
    time: "14:00 11/09/2025",
    pointBefore: 29482,
    pointAfter: 30082,
    changePoint: "+600",
    reasons: "Donated Book: Principles of Neural Science",
  },
  {
    id: 969,
    time: "14:00 11/09/2025",
    pointBefore: 28882,
    pointAfter: 29482,
    changePoint: "+600",
    reasons: "Donated Book: Principles of Neural Science",
  },
  {
    id: 968,
    time: "14:00 11/09/2025",
    pointBefore: 27582,
    pointAfter: 28882,
    changePoint: "+1200",
    reasons: "Donated Book: Proceedings of the Royal Society, 1925",
  },
  {
    id: 967,
    time: "8:30 11/08/2025",
    pointBefore: 27552,
    pointAfter: 27582,
    changePoint: "+30",
    reasons: "Returned Book: A Farewell to Arms in time",
  },
  {
    id: 966,
    time: "8:30 06/07/2025",
    pointBefore: 27522,
    pointAfter: 27552,
    changePoint: "+30",
    reasons:
      "Returned Book: The Sleepwalkers: How Europe Went to War in 1914 in time",
  },
  {
    id: 965,
    time: "8:30 20/06/2025",
    pointBefore: 27492,
    pointAfter: 27522,
    changePoint: "+30",
    reasons: "Returned Book: Germany's Aims in the First World War in time",
  },
  {
    id: 964,
    time: "8:30 12/06/2025",
    pointBefore: 27612,
    pointAfter: 27492,
    changePoint: "-120",
    reasons: "Returned Book: Art of War late",
  },
  {
    id: 963,
    time: "8:30 29/04/2025",
    pointBefore: 27732,
    pointAfter: 27612,
    changePoint: "-120",
    reasons: "Returned Book: The Rise and Fall of the Third Reich late",
  },
];

export function PointHistory() {
  const columns = useMemo<ColumnDef<PointType>[]>(
    () => [
      {
        accessorKey: "id",
        cell: (info) => info.getValue(),
        header: "Id",
        footer: (props) => props.column.id,
      },
      {
        accessorKey: "time",
        cell: (info) => info.getValue(),
        header: "Time",
        footer: (props) => props.column.id,
      },
      {
        accessorKey: "pointBefore",
        cell: (info) => info.getValue(),
        header: "Total Points Before",
        footer: (props) => props.column.id,
      },
      {
        accessorKey: "pointAfter",
        cell: (info) => info.getValue(),
        header: "Total Points After",
        footer: (props) => props.column.id,
      },
      {
        accessorKey: "changePoint",
        cell: (info) => info.getValue(),
        header: "Changes",
        footer: (props) => props.column.id,
      },
      {
        accessorKey: "reasons",
        cell: (info) => info.getValue(),
        header: "Reasons",
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
