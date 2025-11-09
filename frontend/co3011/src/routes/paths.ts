export const rootPaths = {
  userRoot: "/",
  libRoot: "/librarian",
  authRoot: "/authentication",
  adminRoot: "/admin",
};

export default {
  USER: {
    DASHBOARD: rootPaths.userRoot,
    LIBRARY_CATALOG: `${rootPaths.userRoot}catalog`,
    BOOK_CONTRIBUTE: `${rootPaths.userRoot}catalog/contribute`,
    HISTORY: `${rootPaths.userRoot}history`,
    PROFILE: `${rootPaths.userRoot}profile`,
  },
  LIBRARIAN: {
    DASHBOARD: rootPaths.libRoot,
    BOOK_MANAGEMENT: `${rootPaths.libRoot}/book`,
    HISTORY: `${rootPaths.libRoot}/history`,
  },
  ADMIN: {
    DASHBOARD: rootPaths.adminRoot,
    MEMBER_MANAGEMENT: `${rootPaths.adminRoot}/member`,
    REPORT_AND_ANALYTIC: `${rootPaths.adminRoot}/report`,
  },
};
