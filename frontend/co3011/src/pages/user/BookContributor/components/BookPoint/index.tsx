const BookPointItem = ({
  title = "",
  pointText = "",
  bookPointColor,
}: {
  title?: string;
  pointText?: string;
  bookPointColor?: string;
}) => {
  return (
    <div style={{ display: "flex", justifyContent: "space-between" }}>
      <div style={{ display: "flex", alignItems: "center", gap: 10 }}>
        {title}
        <div
          style={{ backgroundColor: bookPointColor, width: 30, height: 10 }}
        ></div>
      </div>
      <p style={{ color: "#27C840" }}>{pointText}</p>
    </div>
  );
};

export function BookPoint() {
  return (
    <>
      <BookPointItem
        title="General Books"
        bookPointColor="#AAAAAA"
        pointText="+200"
      />

      <BookPointItem
        title="Popular Books"
        bookPointColor="#00FF00"
        pointText="+500"
      />

      <BookPointItem
        title="Specialized Books"
        bookPointColor="#00FFFF"
        pointText="+1200"
      />

      <BookPointItem
        title="Limited Copies Books"
        bookPointColor="#FF00FF"
        pointText="+2100"
      />

      <BookPointItem
        title="Unique Books"
        bookPointColor="#FF2400"
        pointText="+3600"
      />
    </>
  );
}
