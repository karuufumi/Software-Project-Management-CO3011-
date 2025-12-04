import { createElement } from "react";
import type { SidebarItem } from "../components/sidebar/sidebar";
import {
  TrendingUp,
  BookMarked,
  History,
  User,
  ChartNoAxesColumn,
} from "lucide-react";
import paths from "./paths";

const sitemap: SidebarItem[] = [
  // Guest
  {
    label: "Dashboard",
    path: paths.GUEST.DASHBOARD,
    role: "guest",
    icon: createElement(TrendingUp),
  },
  {
    label: "History",
    path: paths.GUEST.HISTORY,
    role: "guest",
    icon: createElement(History),
  },
  // Member
  {
    label: "Dashboard",
    path: paths.USER.DASHBOARD,
    role: "user",
    icon: createElement(TrendingUp),
  },
  {
    label: "Library catalog",
    path: paths.USER.LIBRARY_CATALOG,
    role: "user",
    icon: createElement(BookMarked),
  },
  {
    label: "History",
    path: paths.USER.HISTORY,
    role: "user",
    icon: createElement(History),
  },
  // Librarian
  {
    label: "Dashboard",
    path: paths.LIBRARIAN.DASHBOARD,
    role: "lib",
    icon: createElement(TrendingUp),
  },
  {
    label: "Book Management",
    path: paths.LIBRARIAN.BOOK_MANAGEMENT,
    role: "lib",
    icon: createElement(BookMarked),
  },
  {
    label: "History",
    path: paths.LIBRARIAN.HISTORY,
    role: "lib",
    icon: createElement(History),
  },
  {
    label: "Missing Book Handler",
    path: paths.LIBRARIAN.MISSING_BOOK_HANDLER,
    role: "lib",
    icon: createElement(BookMarked),
  },

{
  label: "Progress",
  path: paths.USER.RANKMAP, 
  role: "user",
  icon: createElement(TrendingUp),
},

  // Admin
  {
    label: "Dashboard",
    path: paths.ADMIN.DASHBOARD,
    role: "admin",
    icon: createElement(TrendingUp),
  },
 
];

export default sitemap;
