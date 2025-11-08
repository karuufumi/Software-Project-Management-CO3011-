import type { SidebarItem } from "../components/sidebar/sidebar";

const sitemap: SidebarItem[] = [
  { label: "Dashboard", path: "/", role: "all" },
  { label: "Library catalog", path: "/catalog", role: "user" },
  { label: "History", path: "/history", role: "user" },
];

export default sitemap;
