// src/pages/User/LibraryCatalog/bookService.ts

export async function fetchAllBooks() {
  try {
    const response = await fetch("/api/book");

    if (!response.ok) {
      throw new Error("Failed to fetch books");
    }

    const result = await response.json();
    return result.data; // backend returns { success, data, message }
  } catch (error) {
    console.error("Error fetching books:", error);
    return [];
  }
}
