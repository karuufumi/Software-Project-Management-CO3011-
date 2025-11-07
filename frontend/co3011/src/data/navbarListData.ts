import sitemap from "../routes/sitemap";

export const navList = (role: string) =>
  sitemap.filter((item) => item.role === role || item.role === "all");

export const navFocused = (role: string, elem: string) => {
  const allNav = navList(role);
  return allNav.map((item) => {
    if (item.label.toLowerCase() === elem) {
      return { ...item, active: true };
    } else {
      return item;
    }
  });
};
