import React, { useState } from "react";

interface Book {
  title: string;
  author: string;
  publishedDate: string;
  publisher: string;
  genres: string[];
  description: string;
  copies: number;
  rarity: string;
  imageUrl: string;
  requestQueue: number[];
}

export default function LibrarianBookDetailx() {
  const [book, setBook] = useState<Book>({
    title: "My Struggle",
    author: "Adolf Hitler",
    publishedDate: "18/07/1925",
    publisher: "Franz Eher Nachfolger",
    genres: ["Political Theory", "Autobiography", "Propaganda"],
    description:
      "A political manifesto combining autobiography with Hitler’s ideology and future plans for Germany.",
    copies: 92,
    rarity: "Specialized",
    imageUrl:
      "https://upload.wikimedia.org/wikipedia/commons/5/50/Mein_Kampf_dust_jacket.jpeg",
    requestQueue: [36, 69, 3005, 1964, 18, 8],
  });

  const handleIncrease = () =>
    setBook((b) => ({ ...b, copies: b.copies + 1 }));
  const handleDecrease = () =>
    setBook((b) => ({ ...b, copies: Math.max(0, b.copies - 1) }));

  return (
    <div className="flex min-h-screen bg-gray-100">
      {/* Sidebar */}
      <aside className="w-60 bg-white shadow-md flex flex-col justify-between p-4">
        <div>
          <h1 className="text-xl font-bold mb-8">BK Library</h1>
          <nav className="space-y-3">
            <SidebarItem active label="Book Management" />
            <SidebarItem label="History" />
          </nav>
        </div>

        <div className="flex flex-col items-center border-t pt-4">
          <img
            src="https://via.placeholder.com/60"
            alt="Librarian"
            className="rounded-full mb-2"
          />
          <p className="font-medium text-center text-sm">
            Mr. Librarian <br />
            <span className="text-gray-500 text-xs">
              librarian@hcmut.edu.vn
            </span>
          </p>
          <button className="mt-3 bg-red-400 hover:bg-red-500 text-white px-4 py-1 rounded-md text-sm">
            Log out
          </button>
        </div>
      </aside>

      {/* Main content */}
      <main className="flex-1 p-8">
        <h2 className="text-lg font-semibold mb-6">Librarian Book Detail</h2>

        <div className="bg-white rounded-2xl shadow-md p-6">
          <h3 className="text-base font-semibold mb-4 border-b pb-2">
            Book Detail
          </h3>

          <div className="flex gap-6">
            {/* Left side - book cover */}
            <div className="flex flex-col items-center">
              <img
                src={book.imageUrl}
                alt={book.title}
                className="w-48 rounded-md shadow"
              />
              <button className="mt-3 bg-red-500 hover:bg-red-600 text-white px-3 py-2 rounded-md">
                Remove From Catalogue
              </button>

              <div className="mt-4 bg-gray-50 p-3 rounded-md text-center">
                <p className="font-semibold mb-1">Request queue</p>
                {book.requestQueue.map((q) => (
                  <p key={q} className="text-sm text-gray-700">
                    {q}
                  </p>
                ))}
              </div>
            </div>

            {/* Right side - details */}
            <div className="flex-1 grid grid-cols-2 gap-4">
              <InfoBox label="Author" value={book.author} />
              <GenreBox genres={book.genres} />
              <InfoBox label="Published Year" value={book.publishedDate} />
              <InfoBox label="Publisher" value={book.publisher} />

              {/* Available Copy */}
              <div className="bg-gray-100 rounded-xl p-3 text-center flex flex-col justify-center">
                <p className="font-semibold text-gray-700 mb-2">
                  Available Copy
                </p>
                <div className="flex justify-center items-center gap-3">
                  <button
                    onClick={handleDecrease}
                    className="bg-blue-500 text-white w-7 h-7 rounded-md font-bold"
                  >
                    -
                  </button>
                  <p className="text-lg font-semibold">{book.copies}</p>
                  <button
                    onClick={handleIncrease}
                    className="bg-blue-500 text-white w-7 h-7 rounded-md font-bold"
                  >
                    +
                  </button>
                </div>
              </div>

              {/* Rarity */}
              <InfoBox label="Rarity" value={book.rarity} />

              {/* Description */}
              <div className="bg-gray-100 rounded-xl p-3 col-span-2">
                <p className="font-semibold text-gray-700 mb-1">Description</p>
                <p className="text-gray-600 text-sm">{book.description}</p>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}

function SidebarItem({
  label,
  active,
}: {
  label: string;
  active?: boolean;
}) {
  return (
    <div
      className={`flex items-center gap-2 px-3 py-2 rounded-md cursor-pointer ${
        active ? "bg-blue-100 text-blue-600" : "text-gray-700 hover:bg-gray-100"
      }`}
    >
      <span className="w-2 h-2 rounded-full bg-current"></span>
      <p className="text-sm font-medium">{label}</p>
    </div>
  );
}

function InfoBox({ label, value }: { label: string; value: string }) {
  return (
    <div className="bg-gray-100 rounded-xl p-3 text-center">
      <p className="font-semibold text-gray-700 mb-1">{label}</p>
      <p className="text-gray-800">{value}</p>
    </div>
  );
}

function GenreBox({ genres }: { genres: string[] }) {
  return (
    <div className="bg-gray-100 rounded-xl p-3 text-center">
      <p className="font-semibold text-gray-700 mb-2">Genre</p>
      <div className="flex flex-wrap justify-center gap-2">
        {genres.map((g) => (
          <span
            key={g}
            className="bg-gray-300 text-gray-800 px-3 py-1 rounded-lg text-sm font-medium"
          >
            {g}
          </span>
        ))}
      </div>
    </div>
  );
}
